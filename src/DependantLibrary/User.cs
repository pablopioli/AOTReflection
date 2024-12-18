using System.ComponentModel.DataAnnotations;

namespace Apparatus.AOT.Reflection.Playground
{
    public interface IUser
    {
        List<ItemClass>? Items { get; set; }
    }

    public class User : IUser
    {
        [Required]
        // place to replace 2
        public string FirstName { get; set; } = "";

        [Required]
        public virtual string LastName { get; set; } = "";

        public List<ItemClass>? Items { get; set; }
    }

    public class Admin : User
    {
        public new string FirstName { get; set; } = "";
        public override string LastName { get; set; } = "";
    }

    public class ItemClass
    { }
}