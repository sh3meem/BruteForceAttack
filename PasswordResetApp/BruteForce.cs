using System;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordResetApp.Models
{
    public class BruteForce
    {
        public static string BruteForceAttack(string targetHash, int maxThreads)
        {
            var charset = "abcdefghijklmnopqrstuvwxyz";
            var maxPasswordLength = 4; // Adjust as needed

            var tasks = new Task<string>[maxThreads];
            var foundPassword = string.Empty;
            var cancellationTokenSource = new CancellationTokenSource();

            for (int i = 0; i < maxThreads; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int length = 1; length <= maxPasswordLength; length++)
                    {
                        var result = GeneratePasswords(new char[length], 0, charset, targetHash, cancellationTokenSource.Token);
                        if (result != null)
                        {
                            foundPassword = result;
                            cancellationTokenSource.Cancel();
                            break;
                        }
                    }
                    return foundPassword;
                });
            }

            Task.WaitAll(tasks);
            return foundPassword;
        }

        private static string GeneratePasswords(char[] current, int position, string charset, string targetHash, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return null;
            }

            if (position == current.Length)
            {
                var password = new string(current);
                var hash = PasswordManager.EncryptPassword(password);
                if (hash == targetHash)
                {
                    return password;
                }
                return null;
            }

            foreach (var c in charset)
            {
                current[position] = c;
                var result = GeneratePasswords(current, position + 1, charset, targetHash, token);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}