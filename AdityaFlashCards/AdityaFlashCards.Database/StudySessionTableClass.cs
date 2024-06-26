﻿using AdityaFlashCards.Database.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdityaFlashCards.Database;

internal class StudySessionTableClass
{
    private string? _connectionString;

    public StudySessionTableClass(string? connectionString)
    {
        _connectionString = connectionString;
    }

    internal void CreateTable()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute(@"CREATE TABLE StudySessions (StudySessionId INT PRIMARY KEY IDENTITY(3000,1), Fk_StackID INT NOT NULL FOREIGN KEY REFERENCES Stacks(StackID) ON DELETE CASCADE, SessionDate VARCHAR(20) NOT NULL, SessionScore INT NOT NULL, TotalScore INT NOT NULL)");
    }

    internal void InsertStudySession(int stackID, string date, int score, int totalScore)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        conn.Execute("INSERT INTO StudySessions(Fk_StackID, SessionDate, SessionScore, TotalScore) VALUES (@stackID, @date, @score, @totalScore)", new {stackID, date, score, totalScore});
    }

    internal List<StudySession> GetAllStudySessions()
    {
        List<StudySession> sessions = new List<StudySession>();
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        string sql = "SELECT Stacks.Name AS StackName, StudySessions.SessionDate, StudySessions.SessionScore, StudySessions.TotalScore FROM StudySessions JOIN Stacks ON StudySessions.Fk_StackID = Stacks.StackID";
        var result = conn.Query<StudySession>(sql);
        sessions.AddRange(result);
        return sessions;
    }
}