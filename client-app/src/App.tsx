import { useState, useEffect } from "react";
import { Container } from "react-bootstrap";
import Category from "./interfaces/category";
import axios from "axios";

function App() {
  const [categories, setCategories] = useState<Category[]>([]);

  useEffect(() => {
    axios
      .get("http://localhost:5000/api/categories")
      .then((resp) => {
        console.log(resp.data);
        setCategories(resp.data);
      })
      .catch((error) => {
        console.error(error.message);
      });
  }, []);

  return (
    <Container>
      <h1>Forum App</h1>
      <h2>Categories</h2>
      <>
        {categories.forEach((category) => {
          return (
            <p key={category.id}>
              `${category.name} ${category.description}`
            </p>
          );
        })}
      </>
      <h2>Topics</h2>
    </Container>
  );
}

export default App;
