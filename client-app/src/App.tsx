import { useState, useEffect } from "react";
import { Container } from "react-bootstrap";
import axios from "axios";
import Categories from "./components/Categories";
import Posts from "./components/Posts";
import CategoriesProps from "./interfaces/CategoriesProps";
import Category from "./interfaces/Category";
import Post from "./interfaces/Post";
import PostsProps from "./interfaces/PostsProps";

axios.defaults.baseURL = "http://localhost:5000/api";

function App() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [categoryIndex, setCategoryIndex] = useState(0);
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    axios
      .get("/categories")
      .then((resp) => {
        setCategories(resp.data);
      })
      .catch((error) => {
        console.error(error.message);
      });
  }, []);

  useEffect(() => {
    axios
      .get("/posts")
      .then((resp) => {
        setPosts(resp.data);
      })
      .catch((error) => {
        console.error(error.message);
      });
  }, []);

  const categoriesProps: CategoriesProps = {
    categories: categories,
  };

  const postsProps: PostsProps = {
    posts: posts,
    category: categories[categoryIndex],
  };

  return (
    <Container>
      <h1>Forum App</h1>
      <h2>Categories</h2>
      <Categories {...categoriesProps} />
      <h2>Posts</h2>
      <Posts {...postsProps} />
    </Container>
  );
}

export default App;
