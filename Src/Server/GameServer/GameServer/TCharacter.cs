//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class TCharacter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TCharacter()
        {
            this.MapID = 1;
            this.Items = new HashSet<TCharacterItem>();
        }
    
        public int ID { get; set; }
        public int TID { get; set; }
        public string Name { get; set; }
        public int Class { get; set; }
        public int MapID { get; set; }
        public int MapPosX { get; set; }
        public int MapPosY { get; set; }
        public int MapPosZ { get; set; }
    
        public virtual TPlayer Player { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TCharacterItem> Items { get; set; }
        public virtual TCharacterBag Bag { get; set; }
    }
}
