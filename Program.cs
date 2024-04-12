using System.Text.Json;

namespace json_dz
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // Database class to manage users
    public class UserDatabase
    {
        private List<User> _users;

        public UserDatabase()
        {
            _users = new List<User>();
        }

        public void AddUser(User user)
        {
            user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
            }
        }

        public void DeleteUser(int id)
        {
            _users.RemoveAll(u => u.Id == id);
        }

        public void SaveToJson(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(_users, options);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json);
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new UserDatabase();

            // Add some users
            db.AddUser(new User { Name = "Alice", Email = "alice@example.com" });
            db.AddUser(new User { Name = "Bob", Email = "bob@example.com" });

            // Save users to JSON file
            db.SaveToJson("users.json");

            // Load users from JSON file
            db.LoadFromJson("users.json");

            // Display all users
            foreach (var user in db.GetAllUsers())
            {
                Console.WriteLine($"Id: {user.Id}, Name: {user.Name}, Email: {user.Email}");
            }
        }
    }
}
