using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace NextERP.Util
{
    public class PasswordHasher
    {
        // Số lần lặp để tăng độ khó cho tấn công brute force
        private const int IterationCount = 10000;

        // Kích thước của salt để tăng độ ngẫu nhiên
        private const int SaltSize = 128 / 8;

        // Kích thước của hash output
        private const int HashSize = 256 / 8;

        public static string HashPassword(string password)
        {
            // Tạo một salt ngẫu nhiên
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash mật khẩu với salt sử dụng PBKDF2
            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount,
                numBytesRequested: HashSize
            );

            // Kết hợp salt và hash để lưu trữ trong cơ sở dữ liệu
            byte[] resultBytes = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, resultBytes, 0, SaltSize);
            Buffer.BlockCopy(hashBytes, 0, resultBytes, SaltSize, HashSize);

            // Chuyển đổi thành chuỗi hex để lưu trữ
            return Convert.ToBase64String(resultBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Chuyển đổi chuỗi hex được lưu trữ thành mảng byte
            byte[] storedBytes = Convert.FromBase64String(hashedPassword);

            // Tách salt từ mảng byte
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(storedBytes, 0, salt, 0, SaltSize);

            // Hash mật khẩu nhập vào với salt đã lưu và kiểm tra xem kết quả có khớp không
            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount,
                numBytesRequested: HashSize
            );

            // So sánh từng byte của hash để xác nhận
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i] != storedBytes[i + SaltSize])
                {
                    return false; // Không khớp
                }
            }

            return true; // Khớp
        }
    }
}
