using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDatabase.Models;

public class Storekeeper
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FIO { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Mail { get; set; }

    [ForeignKey("StorekeeperId")]
    public List<Material>? Materials { get; set; }
    
    [ForeignKey("StorekeeperId")]
    public List<Circle>? Circles { get; set; }
    
    [ForeignKey("StorekeeperId")]
    public List<Medal>? Medals { get; set; }
}
