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
    var name = cmd.Name.Trim();

    if (cmd.BranchId is not null)
    {
      if (await db.Contacts.AnyAsync(c => c.BranchId == cmd.BranchId && c.Name == name, ct))
        return Result<ContactDto>.Failure($"Ya existe un contacto llamado '{name}' para esta sucursal.");
    }
    else if (await db.Contacts.AnyAsync(c => c.CustomerId == cmd.CustomerId && c.BranchId == null && c.Name == name, ct))
      return Result<ContactDto>.Failure($"Ya existe un contacto llamado '{name}' para este cliente.");

    var contact = new Contact
    {
      CustomerId = cmd.CustomerId,
      BranchId = cmd.BranchId,
      Name = name,
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
