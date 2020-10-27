using Checkflix.Data.QueryExtensions;
using Checkflix.Models;
using Checkflix.Models.CustomEntities;
using Checkflix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkflix.Data.Persistance
{
    public interface ICheckflixRepository
    {
        void AddFollowing(Following following);
        void AddProduction(Production production);
        void AddProductionCategory(ProductionCategory productionCategory);
        void AddUserProduction(ApplicationUserProduction userProduction);
        void AddVodProduction(VodProduction vodProduction);
        bool AnyProductionsExists();
        Task<IEnumerable<Category>> GetAllCategories();
        Task<PagedList<Production>> GetAllProductions(PostQueryFilters filters);

        Task<IEnumerable<Vod>> GetAllVods();
        Task<Category> GetCategory(int id);
        Task<Production> GetProduction(int id);
        Task<ProductionCategory> GetProductionCategory(int categoryId, int productionId);
        Task<ApplicationUser> GetUserFollowings(string userId);
        Task<PagedList<Production>> GetUserCollection(string userId, UserCollectionFilter userCollectionVM);
        Task<ApplicationUserProduction> GetUserProduction(string userId, int productionId);
        Task<List<ApplicationUserProduction>> GetUserProductionsIds(string userId);
        Task<Vod> GetVod(int id);
        Task<VodCountViewModel> GetVodCount();
        Task<VodProduction> GetVodProduction(int vodId, int productionId);
        bool ProductionsExists(int id);
        void RemoveAllProductions();
        void RemoveProduction(Production production);
        void RemoveUserProduction(ApplicationUserProduction userProduction);
        Task<bool> SaveAll();
        void UpdateCategories(IEnumerable<Category> categories);
        void UpdateProduction(Production production);
        void UpdateProductionCategory(ProductionCategory productionCategory);
        void UpdateVodProduction(VodProduction vodProduction);
        bool ValidateFollowing(string followerId, string followeeId);
        Task<Following> GetFollowing(string followerId, string followeeId);
        void RemoveFollowing(Following following);
        Task<IEnumerable<UserViewModel>> GetUsersSearch(string searchQuery);
        Task<UserViewModel> GetUserData(string userId);
        Task<int> GetUnseenNotificationsCount(string userId);
        Task<IEnumerable<Notification>> GetUnseenNotifications(string userId);
        void UpdateUserNotification(IEnumerable<ApplicationUserNotification> notifications);
        Task<IEnumerable<ApplicationUser>> GetUserFollowersNotMuted(string userId);
        void AddApplicationUserNotification(IEnumerable<ApplicationUserNotification> userNotifications);
        Task<IEnumerable<Notification>> GetNotifications(string userId);
        Task<IEnumerable<string>> GetUsersByNotificationPreferences(NotificationFormViewModel notificationFormViewModel);
        void AddApplicationUserNotification(ApplicationUserNotification applicationUserNotification);
        Task<IEnumerable<ApplicationUserNotification>> GetUnseenUserNotifications(string userId);
        void UpdateFollowing(Following following);
    }
}