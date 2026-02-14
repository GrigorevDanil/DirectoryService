namespace DirectoryService.Contracts.Positions.Requests;

public record RemoveDepartmentFromPositionRequest(Guid[] DepartmentIds);