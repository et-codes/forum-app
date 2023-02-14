import Author from "./Author";
import Category from "./Category";

export default interface Post {
  id: string;
  postCategory: Category;
  author: Author;
  title: string;
  text: string;
  modifiedDate?: Date;
  views: number;
  replies: number;
}
