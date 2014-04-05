using BreakingNewsMX.API.Models;
using BreakingNewsMX.Common;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BreakingNewsMX
{
    public sealed partial class HubPage : Page
    {
        public ObservableDictionary DefaultViewModel { get; set; }

        #region List Properties

        public static Post HeroPost { get; set; }
        public static ObservableCollection<Post> LatestPosts { get; set; }
        public static ObservableCollection<Post> PopularPosts { get; set; }

        #endregion

        private bool isHeroLoaded = false;
        private bool isLatestLoaded = false;
        private bool isPopularLoaded = false;

        private NavigationHelper navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public HubPage()
        {
            InitializeComponent();

            DefaultViewModel = new ObservableDictionary();

            HeroPost = new Post();
            LatestPosts = new ObservableCollection<Post>();
            PopularPosts = new ObservableCollection<Post>();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            App.BreakingNewsClient.GetPopularHeroStory((result) =>
            {
                SmartDispatcher.BeginInvoke(() =>
                {
                    HeroPost = result;

                    DefaultViewModel["imgHeroPhoto"] = HeroPost.FriendlyImage;
                    DefaultViewModel["txtHeroTopic"] = HeroPost.FriendlyTopic;
                    DefaultViewModel["txtHeroContent"] = HeroPost.content;
                    
                    isHeroLoaded = true;

                    if (isHeroLoaded &&
                        isLatestLoaded &&
                        isPopularLoaded)
                    {
                        ToggleLoadingText();
                        ToggleEmptyText();
                    }
                });
            });

            App.BreakingNewsClient.GetLatestPosts((result) =>
            {
                SmartDispatcher.BeginInvoke(() =>
                {
                    LatestPosts.Clear();

                    foreach (Post item in result)
                    {
                        LatestPosts.Add(item);
                    }

                    isLatestLoaded = true;

                    if (isHeroLoaded &&
                        isLatestLoaded &&
                        isPopularLoaded)
                    {
                        ToggleLoadingText();
                        ToggleEmptyText();
                    }
                });
            });

            App.BreakingNewsClient.GetPopularPosts((result) =>
            {
                SmartDispatcher.BeginInvoke(() =>
                {
                    PopularPosts.Clear();

                    foreach (Post item in result)
                    {
                        PopularPosts.Add(item);
                    }

                    isPopularLoaded = true;

                    if (isHeroLoaded &&
                        isLatestLoaded &&
                        isPopularLoaded)
                    {
                        ToggleLoadingText();
                        ToggleEmptyText();
                    }
                });
            });
        }

        private void ToggleLoadingText()
        {
            //this.txtLatestPostsLoading.Visibility = System.Windows.Visibility.Collapsed;
            //this.txtPopularPostsLoading.Visibility = System.Windows.Visibility.Collapsed;

            //this.lstLatestPosts.Visibility = System.Windows.Visibility.Visible;
            //this.lstPopularPosts.Visibility = System.Windows.Visibility.Visible;
        }


        private void ToggleEmptyText()
        {
            //if (LatestPosts.Count == 0)
            //    this.txtLatestPostsEmpty.Visibility = System.Windows.Visibility.Visible;
            //else
            //    this.txtLatestPostsEmpty.Visibility = System.Windows.Visibility.Collapsed;

            //if (PopularPosts.Count == 0)
            //    this.txtPopularPostsEmpty.Visibility = System.Windows.Visibility.Visible;
            //else
            //    this.txtPopularPostsEmpty.Visibility = System.Windows.Visibility.Collapsed;
        }


        /// <summary>
        /// Invoked when a HubSection header is clicked.
        /// </summary>
        /// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            //HubSection section = e.Section;
            //var group = section.DataContext;
            //this.Frame.Navigate(typeof(SectionPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        /// <param name="sender">The GridView or ListView
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            //this.Frame.Navigate(typeof(ItemPage), itemId);
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
