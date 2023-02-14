import PostsProps from "../interfaces/PostsProps";

function Posts(props: PostsProps) {
  return (
    <>
      {props.posts.map((post) => (
        <p key={post.id}>
          Category: {post.postCategory.name}
          Author: {post.author.displayName}
          Title: {post.title}
          Text: {post.text}
        </p>
      ))}
    </>
  );
}

export default Posts;
