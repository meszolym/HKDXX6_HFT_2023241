using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.APIModels
{
    public class PrecinctModel : ObservableObject
    {
        [JsonIgnore]
        int iD;

        public int ID {
            get
            {
                return iD;
            }
            set
            {
                SetProperty(ref iD, value);
            } 
        }

        [JsonIgnore]
        string address;

        public string Address {
            get
            {
                return address;
            }
            set
            {
                SetProperty(ref address, value);
            }
        }

        public override bool Equals(object? obj)
        {
            PrecinctModel b = obj as PrecinctModel;
            if (b == null)
            {
                return false;
            }
            return ID == b.ID && Address == b.Address;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Address);
        }

        public override string ToString()
        {
            return $"{ID} ({Address})";
        }

    }
}
