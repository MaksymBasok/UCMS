using UCMS.Domain.Assignments;

namespace Tests.Data.Assignments;

public static class AssignmentData
{
    public static Assignment FirstAssignment(Guid courseId)
        => Assignment.New(
            new Guid("77777777-7777-7777-7777-777777777777"),
            courseId,
            "Intro Assignment",
            "Complete initial tasks",
            DateTime.UtcNow.AddDays(7));

    public static Assignment SecondAssignment(Guid courseId)
        => Assignment.New(
            new Guid("88888888-8888-8888-8888-888888888888"),
            courseId,
            "Project Proposal",
            "Submit project proposal",
            DateTime.UtcNow.AddDays(10));
}
