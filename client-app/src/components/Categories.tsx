import CategoriesProps from "../interfaces/CategoriesProps";
import { Table } from "react-bootstrap";

function Categories(props: CategoriesProps) {
  return (
    <Table>
      <thead>
        <tr>
          <th></th>
        </tr>
      </thead>
      <tbody>
        {props.categories.map((category) => (
          <tr>
            <td>
              <p key={category.id}>
                <strong>{category.name}</strong>
              </p>
              <p>{category.description}</p>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
}

export default Categories;
