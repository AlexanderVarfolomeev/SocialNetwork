using SocialNetwork.ComplaintsServices.Model;

namespace SocialNetwork.ComplaintsServices;

public interface IComplaintService
{
    Task<ComplaintModelResponse> GetComplaint(Guid complaintId);
    Task<IEnumerable<ComplaintModelResponse>> GetComplaints(Guid userId, int offset = 0, int limit = 10);
    Task<ComplaintModelResponse> AddComplaint(Guid userId, ComplaintModelRequest complaint);
    Task AddReasonForComplaint(Guid userId, Guid complaintId, Guid reasonId);
    Task<IEnumerable<ReasonModelResponse>> GetReasonsForComplaints();
}