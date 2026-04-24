using Application.DTO.Output.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Cabinet
{
    public interface ICabinetServices
    {
        public Task<UserInfoDTO> GetInfoByUser(string email);
    }
}
