using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.MODEL;

namespace SistemaVenta.BLL.Servicios
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Menu> _menuRepositorio;
        private readonly IGenericRepository<MenuRol> _menuRolRepositorio;
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Menu> menuRepositorio, 
            IGenericRepository<MenuRol> menuRolRepositorio, 
            IGenericRepository<Usuario> usuarioRepositorio, 
            IMapper mapper)
        {
            _menuRepositorio = menuRepositorio;
            _menuRolRepositorio = menuRolRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _usuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<Menu> tbMenu = await _menuRepositorio.Consultar();
            IQueryable<MenuRol> tbMenuRol = await _menuRolRepositorio.Consultar();

            try
            {
                IQueryable<Menu> tbResultado = (from u in tbUsuario
                                                join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                                join m in tbMenu on mr.IdMenu equals m.IdMenu
                                                select m).AsQueryable();

                var listaMenus = tbResultado.ToList();
                return _mapper.Map<List<MenuDTO>>(listaMenus);
            }
            catch 
            {
                throw;
            }
        }
    }
}
