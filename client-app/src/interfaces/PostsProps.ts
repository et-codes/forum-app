import Category from "./Category";
import Post from "./Post";

export default interface PostsProps {
  posts: Post[];
  category: Category;
}
