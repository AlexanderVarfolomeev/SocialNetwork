using System.Security.Claims;
using AutoMapper;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Common.Enum;
using SocialNetwork.ComplaintsServices;
using SocialNetwork.ComplaintsServices.Model;
using SocialNetwork.Constants.Security;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.WebAPI.Controllers.Complaints.Models;

namespace SocialNetwork.WebAPI.Controllers.Complaints;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class ComplaintsController : ControllerBase
{
    private readonly IComplaintService _complaintService;
    private readonly IMapper _mapper;

    public ComplaintsController(IComplaintService complaintService, IMapper mapper)
    {
        _complaintService = complaintService;
        _mapper = mapper;
        
    }

    [HttpGet("complaints/{complaintId}")]
    public async Task<ComplaintResponse> GetComplaint([FromRoute] Guid complaintId)
    {
        var complaint = await _complaintService.GetComplaint(complaintId);
        return _mapper.Map<ComplaintResponse>(complaint);
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("comments/{commentId}/complaints")]
    public async Task<ComplaintResponse> AddCommentComplaint([FromRoute] Guid commentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _complaintService.AddComplaint(userId, new ComplaintModelRequest()
        {
            ContentId = commentId, Type = ComplaintType.Comment, CreatorId = userId
        });
        return _mapper.Map<ComplaintResponse>(result);
    }

    [HttpGet("complaints/reasons")]
    public async Task<IEnumerable<ReasonResponse>> GetReasons()
    {
        return _mapper.Map<IEnumerable<ReasonResponse>>(await _complaintService.GetReasonsForComplaints());
    }

    [HttpPost("complaints/{complaintId}/reasons/{reasonId}")]
    public async Task<IActionResult> AddReasonForComplaint([FromRoute] Guid complaintId, [FromRoute] Guid reasonId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _complaintService.AddReasonForComplaint(userId, complaintId, reasonId);
        return Ok();
    }
}