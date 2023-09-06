using AutoMapper;
using Netflow.Entities;
using Netflow.Models;

namespace Netflow.Profiles;

public class WorkflowProfile : Profile
{
    public WorkflowProfile()
    {
        CreateMap<Workflow, WorkflowListItem>();
        CreateMap<Workflow, WorkflowDetail>();
        CreateMap<WorkflowStep, WorkflowDetail.Step>()
            .ForMember(dest => dest.RequiredApprovals, opt =>
            {
                opt.MapFrom(src => src.RequiredApprovals.Select(_ => _.Name));
            });

        CreateMap<WorkflowStepTimeLimitConfiguration, WorkflowDetail.StepTimeLimitConfiguration>();

        CreateMap<WorkflowInstance, WorkflowInstanceListItem>()
            .ForMember(dest => dest.Name, opt =>
            {
                opt.MapFrom(src => src.BaseWorkflow.Name);
            })
            .ForMember(dest => dest.CurrentStep, opt =>
            {
                opt.MapFrom(src => src.IsCompleted ? "" : src.CurrentStep.BaseStep.Name);
            });

        CreateMap<WorkflowInstance, WorkflowInstanceDetail>()
            .ForMember(dest => dest.Name, opt =>
            {
                opt.MapFrom(src => src.BaseWorkflow.Name);
            });

        CreateMap<WorkflowStepInstance, WorkflowInstanceDetail.StepInstance>()
            .ForMember(dest => dest.StepTimeLimit, opt =>
            {
                opt.MapFrom(src => src.WorkflowStepTimeLimit);
            });
        CreateMap<WorkflowStepTimeLimit, WorkflowInstanceDetail.StepTimeLimit>();
    }
}
