using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class AppDbContext : IdentityDbContext<Usuario> {
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

  protected override void OnModelCreating(ModelBuilder configurationBuilder) {
    base.OnModelCreating(configurationBuilder);
  }

  public DbSet<Inmueble> Inmuebles { get; set; } = null!;
}