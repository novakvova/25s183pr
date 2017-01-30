using DBase.Abstact;
using DBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBase.Concrete
{
    public class PostRepository
    {
        IDbContext context;
        public PostRepository(IDbContext context)
        {
            this.context = context;
        }
        public Post AddPost(string name)
        {
            Post post = new Post()
            { Name = name };
            this.context.Set<Post>().Add(post);
            this.SaveChange();
            return post;
        }
        public void AddPostUser(User user, int PostId)
        {
            Post post = this.context.Set<Post>().SingleOrDefault(r => r.Id == PostId);
            if (post != null)
            {
                post.Users.Add(user);
                this.SaveChange();
            }
        }
        public IQueryable<Post> GetAllPosts()
        {
            return this.context.Set<Post>();
        }
        public IList<Post> GetPostsForUser(User user)
        {
            if (user != null)
            {
                return user.Posts.ToList();
            }
            return null;
        }
        public void SaveChange()
        {
            context.SaveChanges();
        }
    }
}
