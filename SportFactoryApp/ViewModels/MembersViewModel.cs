using System;
using System.Collections.ObjectModel;
using System.Linq;
using SportFactoryApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace SportFactoryApp.ViewModels
{
    public class MembersViewModel : BaseViewModel
    {
        public ObservableCollection<Member> Members { get; set; } = new ObservableCollection<Member>();

        public MembersViewModel()
        {
            LoadMembers();
        }

        // Load members from the database using Entity Framework
        public void LoadMembers()
        {
            using (var context = new GymContext())
            {
                var membersList = context.Members.ToList();
                Members.Clear();
                foreach (var member in membersList)
                {
                    Members.Add(member);
                }
            }
        }

        // Add a new member to the database
        public void AddMember(Member newMember)
        {
            try
            {
                using (var context = new GymContext())
                {
                    newMember.StartDate = DateTime.Now;
                    context.Members.Add(newMember);
                    context.SaveChanges();
                }

                Members.Add(newMember);  // Update UI
                MessageBox.Show("Member added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Delete a member from the database
        public void DeleteMember(Member selectedMember)
        {
            try
            {
                using (var context = new GymContext())
                {
                    context.Members.Remove(selectedMember);
                    context.SaveChanges();
                }

                Members.Remove(selectedMember);  // Update UI
                MessageBox.Show("Member deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Update a member in the database
        public void UpdateMember(Member selectedMember)
        {
            try
            {
                using (var context = new GymContext())
                {
                    context.Members.Update(selectedMember);
                    context.SaveChanges();
                }

                MessageBox.Show("Member updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
