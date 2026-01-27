namespace DirectoryService.Contracts.Positions.Requests;

public record AddDepartmentToPositionRequest(Guid[] DepartmentIds);