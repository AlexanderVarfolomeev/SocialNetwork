using System.Diagnostics;
using AutoMapper;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.ComplaintsServices.Model;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.Repository;

namespace SocialNetwork.ComplaintsServices;

public class ComplaintService : IComplaintService
{
    private readonly IRepository<Complaint> _complaintRepository;
    private readonly IMapper _mapper;
    private readonly IAdminService _adminService;
    private readonly IRepository<ReasonForComplaint> _reasonRepository;
    private readonly IRepository<ReasonComplaint> _reasonComplaintRepository;

    public ComplaintService(
        IRepository<Complaint> complaintRepository,
        IMapper mapper,
        IAdminService adminService,
        IRepository<ReasonForComplaint> reasonRepository,
        IRepository<ReasonComplaint> reasonComplaintRepository)
    {
        _complaintRepository = complaintRepository;
        _mapper = mapper;
        _adminService = adminService;
        _reasonRepository = reasonRepository;
        _reasonComplaintRepository = reasonComplaintRepository;
    }

    public async Task AddReasonForComplaint(Guid userId, Guid complaintId, Guid reasonId)
    {
        var complaint = await _complaintRepository.GetAsync(complaintId);
        // TODO add error msg
        ProcessException.ThrowIf(() => complaint.CreatorId != userId, "");
        await _reasonRepository.GetAsync(reasonId);
        await _reasonComplaintRepository.AddAsync(new ReasonComplaint()
        {
            ComplaintId = complaintId,
            ReasonId = reasonId,
        });
    }

    public async Task<IEnumerable<ReasonModelResponse>> GetReasonsForComplaints()
    {
        return _mapper.Map<IEnumerable<ReasonModelResponse>>(await _reasonRepository.GetAllAsync());
    }

    public async Task<ComplaintModelResponse> GetComplaint(Guid complaintId)
    {
        var complaint = await _complaintRepository.GetAsync(complaintId);
        var result = _mapper.Map<ComplaintModelResponse>(complaint);
        var reasonsIds =
            (await _reasonComplaintRepository.GetAllAsync(x => x.ComplaintId == complaintId)).Select(x => x.ReasonId);
        var reasons = await _reasonRepository.GetAllAsync(x => reasonsIds.Contains(x.Id));

        result.ReasonsStrings.AddRange(reasons.Select(x => x.Name));
        return result;
    }

    public async Task<IEnumerable<ComplaintModelResponse>> GetComplaints(Guid userId, int offset = 0, int limit = 10)
    {
        bool isAdmin = await _adminService.IsAdminAsync(userId);
        ProcessException.ThrowIf(() => !isAdmin, ErrorMessages.OnlyAdminCanDoItError);

        var complaints = await _complaintRepository.GetAllAsync(offset, limit);
        List<ComplaintModelResponse> result = new List<ComplaintModelResponse>();
        foreach (var complaint in complaints)
        {
            result.Add(await GetReasonsForComplaint(_mapper.Map<ComplaintModelResponse>(complaint)));
        }
        return result;
    }

    public async Task<ComplaintModelResponse> AddComplaint(Guid userId, ComplaintModelRequest complaint)
    {
        //TODO check ban user
        Complaint newComplaint = new Complaint
        {
            Type = complaint.Type,
            CreatorId = userId
        };

        switch (complaint.Type)
        {
            case ComplaintType.Comment:
                newComplaint.CommentId = complaint.ContentId;
                break;
            case ComplaintType.Group:
                newComplaint.GroupId = complaint.ContentId;
                break;
            case ComplaintType.Post:
                newComplaint.PostId = complaint.ContentId;
                break;
            case ComplaintType.User:
                newComplaint.UserId = complaint.ContentId;
                break;
            default:
                //TODO exception
                break;
        }

        return _mapper.Map<ComplaintModelResponse>(await _complaintRepository.AddAsync(newComplaint));
    }

    private async Task<ComplaintModelResponse> GetReasonsForComplaint(ComplaintModelResponse complaint)
    {
        var reasonsIds =
            (await _reasonComplaintRepository.GetAllAsync(x => x.ComplaintId == complaint.Id)).Select(x => x.ReasonId);
        var reasons = await _reasonRepository.GetAllAsync(x => reasonsIds.Contains(x.Id));

        complaint.ReasonsStrings.AddRange(reasons.Select(x => x.Name));
        return complaint;
    }
}