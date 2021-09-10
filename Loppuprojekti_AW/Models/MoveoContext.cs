using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Loppuprojekti_AW.Models
{
    public partial class MoveoContext : DbContext
    {
        public MoveoContext()
        {
        }

        public MoveoContext(DbContextOptions<MoveoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendee> Attendees { get; set; }
        public virtual DbSet<Enduser> Endusers { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<UsersSport> UsersSports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Attendee>(entity =>
            {
                entity.ToTable("Attendee");

                entity.Property(e => e.Attendeeid).HasColumnName("attendeeid");

                entity.Property(e => e.Organiser).HasColumnName("organiser");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.AttendeesNavigation)
                    .HasForeignKey(d => d.Postid)
                    .HasConstraintName("fk_attendee_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Attendees)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("fk_attendee_enduser");
            });

            modelBuilder.Entity<Enduser>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PK__Enduser__CBA1B25715BBD908");

                entity.ToTable("Enduser");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.Club)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("club");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(320)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Photo)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .HasColumnName("photo");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Userrole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userrole");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Messageid).HasColumnName("messageid");

                entity.Property(e => e.Messagebody)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("messagebody");

                entity.Property(e => e.Receiverid).HasColumnName("receiverid");

                entity.Property(e => e.Senderid).HasColumnName("senderid");

                entity.Property(e => e.Sendtime)
                    .HasColumnType("datetime")
                    .HasColumnName("sendtime");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.Receiverid)
                    .HasConstraintName("FK__Message__receive__52593CB8");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.Senderid)
                    .HasConstraintName("fk_message_sender");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Attendees).HasColumnName("attendees");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(12, 9)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(12, 9)")
                    .HasColumnName("longitude");

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("place");

                entity.Property(e => e.Postname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("postname");

                entity.Property(e => e.Posttype).HasColumnName("posttype");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.Privacy).HasColumnName("privacy");

                entity.Property(e => e.Sportid).HasColumnName("sportid");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.Sportid)
                    .HasConstraintName("fk_post_sport");
            });

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.ToTable("Sport");

                entity.Property(e => e.Sportid).HasColumnName("sportid");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Sportname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sportname");
            });

            modelBuilder.Entity<UsersSport>(entity =>
            {
                entity.ToTable("UsersSport");

                entity.Property(e => e.Userssportid).HasColumnName("userssportid");

                entity.Property(e => e.Accredit)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("accredit");

                entity.Property(e => e.Experience).HasColumnName("experience");

                entity.Property(e => e.Level)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("level");

                entity.Property(e => e.Sportid).HasColumnName("sportid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.UsersSports)
                    .HasForeignKey(d => d.Sportid)
                    .HasConstraintName("fk_userssport_sport");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersSports)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("fk_userssport_enduser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
