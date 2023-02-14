import CategoriesProps from "../interfaces/CategoriesProps";

function Categories(props: CategoriesProps) {
  return (
    <>
      {props.categories.map((category) => {
        return (
          <p key={category.id}>
            {category.name}: {category.description}
          </p>
        );
      })}
    </>
  );
}

export default Categories;
