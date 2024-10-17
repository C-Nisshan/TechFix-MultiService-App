using Supplier_backend.Dtos;
using Supplier_backend.Models;
using Supplier_backend.Data;
using Microsoft.EntityFrameworkCore;
using Supplier_backend.Models;

namespace Supplier_backend.Services
{
    public class ClientService
    {
        private readonly SupplierDbContext _context;

        public ClientService(SupplierDbContext context)
        {
            _context = context;
        }

        // Service method to get all clients
        public async Task<List<ClientDto>> GetClientsAsync()
        {
            var clients = await _context.Clients
                                        .Select(c => new ClientDto
                                        {
                                            ClientId = c.ClientId,
                                            CompanyName = c.CompanyName,
                                            ContactInfo = c.ContactInfo
                                        })
                                        .ToListAsync();

            return clients;
        }

        // Service method to add a new client
        public async Task<ClientDto> AddClientAsync(ClientDto clientDto)
        {
            var client = new Client
            {
                CompanyName = clientDto.CompanyName,
                ContactInfo = clientDto.ContactInfo
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return new ClientDto
            {
                ClientId = client.ClientId,
                CompanyName = client.CompanyName,
                ContactInfo = client.ContactInfo
            };
        }

        // Service method to update an existing client
        public async Task<ClientDto?> UpdateClientAsync(int id, ClientDto clientDto)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return null;
            }

            client.CompanyName = clientDto.CompanyName;
            client.ContactInfo = clientDto.ContactInfo;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return new ClientDto
            {
                ClientId = client.ClientId,
                CompanyName = client.CompanyName,
                ContactInfo = client.ContactInfo
            };
        }

        // Service method to delete a client
        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return false;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
