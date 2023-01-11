# BaseModel

- Guid Id
- DateTime CreatedDate

# User : BaseModel

- string Name
- string PasswordHash
- DateTime LastLogin
- DateTime LastLogout

# Category : BaseModel

- string Name
- string Description

# Post : BaseModel

- Category PostCategory
- User Author
- Post InReplyTo
- string Text
- DateTime ModifiedDate
