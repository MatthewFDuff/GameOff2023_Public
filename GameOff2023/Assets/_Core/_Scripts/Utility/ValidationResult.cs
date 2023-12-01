using System.Collections.Generic;

namespace _Core._Scripts.Utility
{
    public struct ValidationResult
    {
        public string Name { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool IsValid => ErrorMessages.Count == 0;
        public string CombinedMessages => $"{Name}\n{string.Join("\n", ErrorMessages)}";

        /// <summary>
        /// Creates a new ValidationResult and adds the initial error message.
        /// </summary>
        /// <param name="name">Should be the name of the class that is being validated.</param>
        public ValidationResult(string name)
        {
            Name = name;
            ErrorMessages = new List<string>();
        }
    }
}