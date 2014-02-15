﻿using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using GitReleaseNotes.IssueTrackers;
using GitReleaseNotes.IssueTrackers.GitHub;
using Xunit;

namespace GitReleaseNotes.Tests
{
    [UseReporter(typeof(DiffReporter))]
    public class ReleaseNotesGeneratorTests
    {
        private readonly ReleaseNotesGenerator _sut;

        public ReleaseNotesGeneratorTests()
        {
            _sut = new ReleaseNotesGenerator();
        }

        [Fact]
        public void ApproveSimpleTests()
        {
            var arguments = new GitReleaseNotesArguments
            {
                OutputFile = "ReleaseFile.md"
            };
            var releaseNotes = new SemanticReleaseNotes(new[]
            {
                new SemanticRelease("", null, new[]
                {
                    new ReleaseNoteItem("Issue 1", "#1", new Uri("http://github.com/org/repo/issues/1"), new string[0])
                })
            });

            var result =_sut.GenerateReleaseNotes(arguments, releaseNotes);

            Approvals.Verify(result);
        }

        [Fact]
        public void ItemIsCategorised()
        {
            var arguments = new GitReleaseNotesArguments
            {
                OutputFile = "ReleaseFile.md"
            };
            var releaseNotes = new SemanticReleaseNotes(new[]
            {
                new SemanticRelease("", null, new[]
                {
                    new ReleaseNoteItem("Issue 1", "#1", new Uri("http://github.com/org/repo/issues/1"),
                        new[] {"feature"})
                })
            });

            var result = _sut.GenerateReleaseNotes(arguments, releaseNotes);

            Approvals.Verify(result);
        }
        
        [Fact]
        public void MultipleReleases()
        {
            var arguments = new GitReleaseNotesArguments
            {
                OutputFile = "ReleaseFile.md"
            };
            var releaseNotes = new SemanticReleaseNotes(new[]
            {
                new SemanticRelease("", null, new[]
                {
                    new ReleaseNoteItem("Issue 1", "#1", new Uri("http://github.com/org/repo/issues/1"),
                        new[] {"feature"})
                }),
                new SemanticRelease("1.2.0", new DateTimeOffset(2013, 12, 06, 0,0,0, new TimeSpan()), new []
                {
                    new ReleaseNoteItem("Issue 2", "#2", new Uri("http://github.com/org/repo/issues/2"),
                        new[] {"feature"}),
                        new ReleaseNoteItem("Issue 3", "#3", new Uri("http://github.com/org/repo/issues/3"),
                        new[] {"bug"})
                })
            });

            var result = _sut.GenerateReleaseNotes(arguments, releaseNotes);

            Approvals.Verify(result);
        }

        [Fact]
        public void LabelOfBugIsCategorisedAsFix()
        {
            var arguments = new GitReleaseNotesArguments
            {
                OutputFile = "ReleaseFile.md"
            };
            var releaseNotes = new SemanticReleaseNotes(new[]
            {
                new SemanticRelease("", null, new[]
                {
                    new ReleaseNoteItem("Issue 1", "#1", new Uri("http://github.com/org/repo/issues/1"), new[] {"bug"})
                })
            });

            var result = _sut.GenerateReleaseNotes(arguments, releaseNotes);

            Approvals.Verify(result);
        }

        [Fact]
        public void AdditionalCategoriesCanBeSpecifiedOnCommandLine()
        {
            var arguments = new GitReleaseNotesArguments
            {
                Categories = "internal refactoring",
                OutputFile = "ReleaseFile.md"
            };
            var releaseNotes = new SemanticReleaseNotes(new[]
            {
                new SemanticRelease("", null, new[]
                {
                    new ReleaseNoteItem("Issue 1", "#1", new Uri("http://github.com/org/repo/issues/1"),
                        new[] {"internal refactoring"})
                })
            });

            var result = _sut.GenerateReleaseNotes(arguments, releaseNotes);

            Approvals.Verify(result);
        }
    }
}