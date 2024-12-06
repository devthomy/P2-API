public class Defi
{
    public int DefiId { get; set; }
    public int? ChallengeId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int? Points { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastUpdated { get; set; } = DateTime.Now;
    
    // Type of challenge - True/False or Multiple Choice
    public DefiType Type { get; set; }
    
    // The actual question text
    public required string QuestionText { get; set; }
    
    // For True/False questions
    public bool? CorrectAnswer { get; set; }
    
    // For Multiple Choice questions (max 4 options)
    public string? Option1 { get; set; }
    public string? Option2 { get; set; }
    public string? Option3 { get; set; }
    public string? Option4 { get; set; }
    public int? CorrectOptionNumber { get; set; }

    // Navigation property
    public Challenge? Challenge { get; set; }
}

public enum DefiType
{
    TrueFalse,
    MultipleChoice
}
