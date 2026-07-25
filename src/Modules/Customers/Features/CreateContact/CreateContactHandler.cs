using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Domain;
using SPSApi.Modules.Customers.Dtos;
using SPSApi.Modules.Customers.Infrastructure;
using SPSApi.Shared.Primitives;

namespace SPSApi.Modules.Customers.Features.CreateContact;

public class CreateContactHandler(CustomersDbContext db)
{
  public async Task<Result<ContactDto>> HandleAsync(CreateContactCommand cmd, CancellationToken ct)
  {
    if (cmd.CustomerId is not null && !await db.Customers.AnyAsync(c => c.Id == cmd.CustomerId, ct))
      return Result<ContactDto>.Failure($"No existe un cliente con Id {cmd.CustomerId}.");

    if (cmd.BranchId is not null && !await db.Branches.AnyAsync(b => b.Id == cmd.BranchId, ct))
      return Result<ContactDto>.Failure($"No existe una sucursal con Id {cmd.BranchId}.");

    var contact = new Contact
    {
      CustomerId = cmd.CustomerId,
      BranchId = cmd.BranchId,
      Name = cmd.Name.Trim(),
      Email = cmd.Email?.Trim(),
      Phone = cmd.Phone?.Trim(),
      Position = cmd.Position?.Trim()
    };

    db.Contacts.Add(contact);
    await db.SaveChangesAsync(ct);

    return Result<ContactDto>.Success(new ContactDto(
      contact.Id, contact.CustomerId, contact.BranchId, contact.Name,
      contact.Email, contact.Phone, contact.Position
    ));
  }
}
