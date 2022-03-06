using Firebase.Auth;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ExamKeeper.JerryH.Jutils
{
    /// <summary>
    /// 與firebase做檔案存取(單例)
    /// </summary>
    public class FirebaseManager
    {
        private FirebaseAuthProvider AuthProvider { get; set; }
        private FirebaseAuthLink AuthLink { get; set; }

        private readonly string DomainName;

        public FirebaseManager(string key, string domainName)
        {
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(key));
            DomainName = domainName;
        }

        /// <summary>
        /// 上傳檔案。回傳url或是失敗訊息。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> UploadData(string fileName, string destinationPath = "Uncategorized")
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    return "找不到檔案";
                }
                return await ConnectFirebase(fileName, destinationPath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> ConnectFirebase(string sourcePath, string destinationPath)
        {
            await RefreshToken();
            string file = sourcePath;

            using Stream stream = File.Open(file, FileMode.Open);
            string fileName = Path.GetFileName(file);

            return await new FirebaseStorage(DomainName,
                                             new FirebaseStorageOptions()
                                             {
                                                 AuthTokenAsyncFactory = () => Task.Run(() => AuthLink.FirebaseToken),
                                                 ThrowOnCancel = true
                                             }).Child(destinationPath)
                                               .Child(fileName)
                                               .PutAsync(stream);
        }

        /// <summary>
        /// 拿firebase的token
        /// </summary>
        /// <returns></returns>
        private async Task RefreshToken()
        {
            if (AuthLink == null)
            {
                //匿名登入(待優化)
                AuthLink = await AuthProvider.SignInAnonymouslyAsync();
            }
            if (AuthLink.IsExpired())
            {
                AuthLink = await AuthProvider.RefreshAuthAsync(AuthLink);
            }
        }
    }
}
