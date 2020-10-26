using Checkflix.Data.QueryExtensions;
using Checkflix.Models;
using Checkflix.Models.CustomEntities;
using Checkflix.ViewModels;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Data.Persistance
{
    public class CheckflixRepository : ICheckflixRepository
    {
        private readonly ApplicationDbContext _context;
        public CheckflixRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Productions
        // Productions
        public async Task<PagedList<Production>> GetAllProductions(PostQueryFilters filters)
        {
            var productions = new List<Production>();
            if (filters.Categories != null)
            {
                productions = await _context.ProductionCategories
                                .Where(x => filters.Categories
                                .Contains(x.CategoryId))
                                .Select(x => x.Production)
                                .Distinct()
                                .Include(m => m.VodProductions)
                                .ToListAsync();
            }
            else
            {
                productions = await _context.Productions
                                .Include(x => x.VodProductions)
                                .ToListAsync();
            }
            // Here in if condition implement filter logic for each patameter in PostQueryFilters class
            // Search string
            if (!string.IsNullOrEmpty(filters.SearchQuery))
            {
                var querySearch = filters.SearchQuery.ToLower();
                productions = productions.Where(x => x.Title.ToLower().Contains(querySearch) || x.Subtitle.ToLower().Contains(querySearch)).ToList();
            }
            // Vod platform // if filters have query like isHbo = false - filter netflix
            if (!filters.IsHbo || !filters.IsNetflix)
            {
                if (!filters.IsHbo)
                {
                    productions = productions.Where(x => x.VodProductions.Any(vp => vp.VodId.Equals(1))).ToList();
                }
                else if (!filters.IsNetflix)
                {
                    productions = productions.Where(x => x.VodProductions.Any(vp => vp.VodId.Equals(2))).ToList();
                }
            }
            // Year from to filter
            if (filters.YearFrom != null && filters.YearTo != null)
            {
                productions = productions.Where(x => x.ReleaseDate.Year >= filters.YearFrom && x.ReleaseDate.Year <= filters.YearTo).ToList();
            }
            // Rating from to filter
            if (filters.RatingFrom != null && filters.RatingTo != null)
            {
                productions = productions.Where(x => x.ImbdRating >= filters.RatingFrom && x.ImbdRating <= filters.RatingTo).ToList();
            }

            var pagedProductions = PagedList<Production>.Create(productions, filters.PageNumber, filters.PageSize);

            return pagedProductions;
        }

        public async Task<Production> GetProduction(int id)
        {
            return await _context.Productions
                .Where(m => m.ProductionId.Equals(id))
                .Include(m => m.VodProductions)
                .ThenInclude(m => m.Vod)
                .Include(m => m.ProductionCategories)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync();
        }

        public void AddProduction(Production production)
        {
            _context.Productions.Add(production);
        }

        public void RemoveProduction(Production production)
        {
            _context.Productions.Remove(production);
        }

        public void UpdateProduction(Production production)
        {
            _context.Productions.Update(production);
        }

        public bool ProductionsExists(int id)
        {
            return _context.Productions.Any(e => e.ProductionId == id);
        }

        public void RemoveAllProductions()
        {
            _context.Productions.RemoveRange(_context.Productions);
        }
        public bool AnyProductionsExists()
        {
            return _context.Productions.Any();
        }

        #endregion

        #region Categories
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public void UpdateCategories(IEnumerable<Category> categories)
        {
            foreach (var c in categories)
            {
                _context.Categories.Add(c);
            }
        }
        #endregion

        #region Vods
        public async Task<Vod> GetVod(int id)
        {
            return await _context.Vods.FindAsync(id);
        }
        public async Task<IEnumerable<Vod>> GetAllVods()
        {
            return await _context.Vods.ToListAsync();
        }

        public async Task<VodCountViewModel> GetVodCount()
        {
            var netflixCount = await _context.VodProductions.Where(v => v.VodId.Equals(1)).CountAsync();
            var hboCount = await _context.VodProductions.Where(v => v.VodId.Equals(2)).CountAsync();

            var countViewModel = new VodCountViewModel
            {
                NetflixCount = netflixCount,
                HboCount = hboCount
            };

            return countViewModel;
        }
        #endregion

        #region VodProduction
        public void AddVodProduction(VodProduction vodProduction)
        {
            _context.VodProductions.Add(vodProduction);
        }

        public async Task<VodProduction> GetVodProduction(int vodId, int productionId)
        {
            return await _context.VodProductions.Where(m => m.VodId.Equals(vodId) && m.ProductionId.Equals(productionId)).FirstOrDefaultAsync();
        }

        public async void UpdateVodProduction(VodProduction vodProduction)
        {
            var production2update = await _context.VodProductions.Where(m => m.ProductionId.Equals(vodProduction.ProductionId) && m.VodId.Equals(vodProduction.VodId)).FirstOrDefaultAsync();


            _context.Entry(production2update).CurrentValues.SetValues(production2update);
            //_context.VodProductions.Update(vodProduction);
        }

        #endregion

        #region ProductionCategory
        public void AddProductionCategory(ProductionCategory productionCategory)
        {
            _context.ProductionCategories.Add(productionCategory);
        }

        public async Task<ProductionCategory> GetProductionCategory(int categoryId, int productionId)
        {
            return await _context.ProductionCategories.Where(m => m.CategoryId.Equals(categoryId) && m.ProductionId.Equals(productionId)).FirstOrDefaultAsync();
        }

        public void UpdateProductionCategory(ProductionCategory productionCategory)
        {
            _context.ProductionCategories.Update(productionCategory);
        }

        #endregion

        #region UserProductions
        public async Task<ApplicationUserProduction> GetUserProduction(string userId, int productionId)
        {
            return await _context.ApplicationUserProductions.Include(m => m.Production).Where(m => m.ApplicationUserId.Equals(userId) && m.ProductionId.Equals(productionId)).FirstOrDefaultAsync();
        }
        public async Task<List<ApplicationUserProduction>> GetUserProductionsIds(string userId)
        {
            return await _context.ApplicationUserProductions.Where(m => m.ApplicationUserId.Equals(userId)).ToListAsync();
        }
        public async Task<PagedList<Production>> GetUserCollection(string userId, UserCollectionFilter userCollectionVM)
        {
            var userCollection = new List<Production>();
            if (userCollectionVM.Favourites == true)
            {
                userCollection = await _context.ApplicationUserProductions.Where(m => m.ApplicationUserId.Equals(userId) && m.Favourites == true).Select(m => m.Production).ToListAsync();
            }
            else if (userCollectionVM.ToWatch == true)
            {
                userCollection = await _context.ApplicationUserProductions.Where(m => m.ApplicationUserId.Equals(userId) && m.ToWatch == true).Select(m => m.Production).ToListAsync();
            }
            else if (userCollectionVM.Watched == true)
            {
                userCollection = await _context.ApplicationUserProductions.Where(m => m.ApplicationUserId.Equals(userId) && m.Watched == true).Select(m => m.Production).ToListAsync();
            }
            var pagedCollection = PagedList<Production>.Create(userCollection, userCollectionVM.PageNumber, userCollectionVM.PageSize);
            return pagedCollection;
        }
        public void AddUserProduction(ApplicationUserProduction userProduction)
        {
            _context.ApplicationUserProductions.Add(userProduction);
        }
        public void RemoveUserProduction(ApplicationUserProduction userProduction)
        {
            _context.ApplicationUserProductions.Remove(userProduction);
        }
        #endregion

        #region Followings 
        public async Task<ApplicationUser> GetUserFollowings(string userId)
        {
            return await _context.Users.Where(x => x.Id.Equals(userId))
                            .Include(m => m.Followers)
                            .ThenInclude(x => x.Follower)
                            .Include(m => m.Followees)
                            .ThenInclude(x => x.Followee)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUserFollowers(string userId)
        {
            return await _context.Followings
                            .Include(x => x.Follower)
                            .Where(x => x.FolloweeId.Equals(userId))
                            .Select(x => x.Follower)
                            .ToListAsync();
        }

        public async Task<Following> GetFollowing(string followerId, string followeeId)
        {
            return await _context.Followings.Where(x => x.FollowerId.Equals(followerId) && x.FolloweeId.Equals(followeeId)).FirstOrDefaultAsync();
        }

        public void AddFollowing(Following following)
        {
            _context.Followings.Add(following);
        }

        public void RemoveFollowing(Following following)
        {
            _context.Followings.Remove(following);
        }

        public bool ValidateFollowing(string followerId, string followeeId)
        {
            return _context.Followings.Any(x => x.FollowerId.Equals(followerId) && x.FolloweeId.Equals(followeeId));
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersSearch(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var querySearch = searchQuery.ToLower();
                var users = await _context.Users.Where(x => x.UserName.Contains(querySearch) || x.Email.ToLower().Contains(querySearch))
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email
                })
                .Take(10)
                .ToListAsync();
                return users;
            }
            return null;
        }
        public async Task<UserViewModel> GetUserData(string userId)
        {
            var user = await _context.Users.Where(x => x.Id.Equals(userId))
               .Select(x => new UserViewModel
               {
                   Id = x.Id,
                   UserName = x.UserName,
                   Email = x.Email
               })
               .FirstOrDefaultAsync();
            return user;
        }
        #endregion

        #region Notifications
        public async Task<int> GetUnseenNotificationsCount(string userId)
        {
            var notificationsCount = await _context.ApplicationUserNotifications
            .Include(x => x.Notification)
            .Where(x => x.ApplicationUserId.Equals(userId) && x.IsSeen.Equals(false))
            .Select(x => x.Notification)
            .CountAsync();

            return notificationsCount;
        }
        public async Task<IEnumerable<Notification>> GetUnseenNotifications(string userId)
        {
            var notifications = await _context.ApplicationUserNotifications
                        .Include(x => x.Notification)
                        .Where(x => x.ApplicationUserId.Equals(userId) && x.IsSeen.Equals(false))
                        .Select(x => x.Notification)
                        .ToListAsync();

            return notifications;
        }

        public async Task<IEnumerable<ApplicationUserNotification>> GetUnseenUserNotifications(string userId)
        {
            var notifications = await _context.ApplicationUserNotifications
                        .Where(x => x.ApplicationUserId.Equals(userId) && x.IsSeen.Equals(false))
                        .ToListAsync();

            return notifications;
        }
        public async Task<IEnumerable<Notification>> GetNotifications(string userId)
        {
            var notifications = await _context.ApplicationUserNotifications
                        .Include(x => x.Notification)
                        .Where(x => x.ApplicationUserId.Equals(userId) && x.IsSeen.Equals(true))
                        .Select(x => x.Notification)
                        .ToListAsync();

            return notifications;
        }

        public async Task<IEnumerable<string>> GetUsersByNotificationPreferences(NotificationFormViewModel notificationFormViewModel)
        {
            if (notificationFormViewModel.ToAll)
            {
                return await _context.Users
                                    .Select(x => x.Id)
                                    .ToListAsync(); ;
            }
            var categoryUsers = new List<string>();
            var vodsUsers = new List<string>();
            if (notificationFormViewModel.Categories.Count > 0)
            {
                foreach (var category in notificationFormViewModel.Categories)
                {
                    categoryUsers.AddRange(_context.ApplicationUserCategories.Where(x => x.CategoryId.Equals(category.CategoryId)).Select(x => x.ApplicationUserId).ToList());
                }
            }

            if (notificationFormViewModel.Vods.Count > 0)
            {
                foreach (var vod in notificationFormViewModel.Vods)
                {
                    vodsUsers.AddRange(_context.ApplicationUserVods.Where(x => x.VodId.Equals(vod.VodId)).Select(x => x.ApplicationUserId).ToList());
                }
            }

            return categoryUsers.Union(vodsUsers).ToList();
        }
        public void UpdateUserNotification(IEnumerable<ApplicationUserNotification> notifications)
        {
            _context.ApplicationUserNotifications.UpdateRange(notifications);
        }
        public void AddApplicationUserNotification(ApplicationUserNotification applicationUserNotification)
        {
            _context.ApplicationUserNotifications.Add(applicationUserNotification);
        }
        public void AddApplicationUserNotification(IEnumerable<ApplicationUserNotification> userNotifications)
        {
            _context.ApplicationUserNotifications.AddRange(userNotifications);
        }
        #endregion

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
