using System.Diagnostics;
using System.Globalization;
using LibGit2Sharp;

//const string pathToRepository = "/home/dmytro/work/poc-git-library/test_git_library";
//const string pathToRepository = "/home/dmytro/work/poc-git-library/git_clone_automated/libgit2sharpTest";
const string pathToRepository = "/app/libgit2sharpTest";
var passwordCredentials = new UsernamePasswordCredentials()
{
    Username = "dimon4egkl55@gmail.com",
    Password = "password"
};

if (Directory.Exists(pathToRepository))
{
    Directory.Delete(pathToRepository, true);
}
Stopwatch stopwatch = new();
stopwatch.Start();
Clone(passwordCredentials, pathToRepository);
stopwatch.Stop();
Console.WriteLine($"Repository cloned in {stopwatch.Elapsed.TotalSeconds} seconds.");

stopwatch.Restart();
using var repository = new Repository(pathToRepository);
Commit(pathToRepository, repository);
Push(passwordCredentials, repository);
DumpHistory(repository);
stopwatch.Stop();

Console.WriteLine($"Connecting, commiting, pushing and dumping logs took: {stopwatch.Elapsed.Milliseconds} milliseconds.");

void Commit(string s, Repository repo)
{
    DateTime localDate = DateTimeOffset.Now.LocalDateTime;
    var contentOfFile = $"content with timestamp {localDate}";
    File.WriteAllText($"{s}/test.txt", contentOfFile);
    RepositoryStatus repositoryStatus = repo.RetrieveStatus();
    repo.Index.Add("test.txt");
    repo.Index.WriteToTree(); // totally need this one


    Signature author = new Signature("dmytro", "dimon4egkl55@gmail.com", localDate);
    var commitMessage = $"another test commit {localDate}";
    var newCommit = repo.Commit(commitMessage, author, author);
    Console.WriteLine($"Committed with message: {commitMessage} and commitId: {newCommit.Id}");
}

void Push(UsernamePasswordCredentials usernamePasswordCredentials, Repository repository2)
{
    var options = new PushOptions
    {
        CredentialsProvider = (_, _, _) => usernamePasswordCredentials
    };
    repository2.Network.Push(repository2.Branches["master"], options);
    Console.WriteLine("Commit was pushed");
}

void DumpHistory(Repository repository3)
{
    IEnumerable<Commit> lastCommits = repository3.Commits.Take(15);
    Console.WriteLine("Dumping commits history...");
    foreach (var commit in lastCommits)
    {
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
            "{0} {1}",
            commit.Id.ToString(7),
            commit.MessageShort));
    }
}

void Clone(UsernamePasswordCredentials passwordCredentials1, string pathToRepository1)
{
    CloneOptions cloneOptions = new CloneOptions();
    cloneOptions.CredentialsProvider =
        (_, _, _) => passwordCredentials1;

    Console.WriteLine("Cloning the repository...");
    Repository.Clone(
        "https://github.com/dimon4egkl/libgit2sharpTest.git",
        pathToRepository1,
        cloneOptions);
}
