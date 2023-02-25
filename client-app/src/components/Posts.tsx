import PostsProps from "../interfaces/PostsProps";
import { Table } from "react-bootstrap";

function Posts(props: PostsProps) {
  return (
    <Table>
      <thead>
        <tr>
          <th>Subject</th>
          <th>Started By</th>
          <th>Replies</th>
          <th>Views</th>
        </tr>
      </thead>
      <tbody>
        {props.posts.map((post) => {
          console.log(post);
          if (post.postCategory.id === props.category.id) {
            return (
              <tr key={post.id}>
                <td>{post.title}</td>
                <td>{post.author.displayName}</td>
                <td>{post.replies}</td>
                <td>{post.views}</td>
              </tr>
            );
          } else {
            return null;
          }
        })}
      </tbody>
    </Table>
  );
}

export default Posts;
