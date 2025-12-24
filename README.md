# Ventro API - E-Commerce Backend

**Ventro** is a full-featured e-commerce backend API built with **ASP.NET Core Web API**, following modern software development best practices including **Clean Architecture** and **SOLID principles**.

The project provides all essential functionalities for a modern online store: product management, shopping basket, wishlist, orders, payments, authentication, and more.

### Live API
- **Base URL**: `https://ventro.runasp.net/`
- Hosted on **MonsterASP.NET** (free ASP.NET hosting)
- Fully tested using **Postman** and **Swagger**

> Note: Swagger UI may not be enabled on the live server, but it's fully available when running locally.

### Technologies & Libraries Used

- **ASP.NET Core Web API**
- **Entity Framework Core** (Code-First)
- **Unit of Work** + **Generic Repository Pattern**
- **Specification Pattern** for dynamic queries
- **AutoMapper** for object mapping
- **Clean Architecture** (Layers: Core, Infrastructure, Application/Services, Presentation/API)
- **SOLID Principles** & various Design Patterns
- **JWT Authentication** + **ASP.NET Core Identity**
- **Redis** (used for storing customer shopping baskets)
- **Stripe** integration for payment processing (Payment Intent creation)
- **Email Service** (confirmation, password reset, etc.)
- **External Authentication**: Sign-in with **Google** and **Facebook**
- **Image Management**: Uploads stored in `wwwroot/images` folder on the server
- **PDF Invoice Generation** for orders
- **Filtering, Sorting, and Pagination** for product listings

### API Endpoints

All endpoints return consistent responses using a custom `ApiResponse<T>` wrapper (with `success`, `message`, `data`, and optional `meta` for pagination).

#### Product Controller (`/api/product`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| GET    | `/getallproducts`                 | Get all products (with filtering, sorting, pagination) | No            |
| POST   | `/addproduct`                     | Add new product (with multiple images)           | No (Admin?)   |
| GET    | `/getproductbyid/{id}`            | Get product by ID                                | No            |
| DELETE | `/deleteproduct/{id}`             | Delete product (deletes images too)               | No (Admin?)   |
| PUT    | `/updateproduct`                  | Update product (replaces images)                  | No (Admin?)   |
| GET    | `/home`                           | Get home page data (Latest, Featured, Offers)     | No            |

#### Wishlist Controller (`/api/WishList`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| POST   | `/add-wishlist?productId={id}`    | Add product to current user's wishlist           | Yes           |
| GET    | `/get-wishlists-for-current-user` | Get wishlist for current user                     | Yes           |
| DELETE | `/remove-from-wishlist?productId={id}` | Remove product from wishlist                | Yes           |
| GET    | `/is-product-in-wishlist?productId={id}` | Check if product is in wishlist           | Yes           |

#### Basket Controller (`/api/basket`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| GET    | `/CreateBasketId`                 | Create and return new basket ID                   | No            |
| POST   | `/AddToBasket`                    | Add item to basket (Redis)                       | No            |
| DELETE | `/RemoveItemFromBasket?basketId={id}&productId={id}` | Remove item from basket         | No            |
| GET    | `/GetBasket?basketId={id}`        | Get basket by ID                                 | No            |
| DELETE | `/ClearBasket?basketId={id}`      | Clear entire basket                              | No            |

#### Order Controller (`/api/Order`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| POST   | `/create-order`                   | Create order from basket                         | Yes           |
| GET    | `/get-order?id={id}`              | Get specific order by ID (user-owned)            | Yes           |
| GET    | `/get-orders-for-user`            | Get all orders for current user                  | Yes           |
| GET    | `/{orderId}/invoice-pdf`          | Download order invoice as PDF                    | Yes           |

#### Payment Controller (`/api/payment`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| POST   | `/Create-payment-intent?basketId={id}` | Create Stripe Payment Intent                | No            |

#### Delivery Method Controller (`/api/DeliveryMethod`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| GET    | `/get-all-delivery-methods`       | Get all delivery methods                         | No            |
| POST   | `/add-delivery-method`            | Add new delivery method                          | No (Admin?)   |
| GET    | `/get-delivery-method?id={id}`    | Get delivery method by ID                        | No            |
| PUT    | `/update-delivery-method`         | Update delivery method                           | No (Admin?)   |
| DELETE | `/delete-delivery-method?id={id}` | Delete delivery method                           | No (Admin?)   |

#### Category Controller (`/api/category`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| GET    | `/get-all-categories`             | Get all categories                               | No            |
| GET    | `/get-category/{id}`              | Get category by ID                               | No            |
| POST   | `/add-category`                   | Add new category                                 | No (Admin?)   |
| PUT    | `/update-category`                | Update category                                  | No (Admin?)   |
| DELETE | `/delete-category/{id}`           | Delete category                                  | No (Admin?)   |

#### Auth Controller (`/api/Auth`)

| Method | Endpoint                          | Description                                      | Auth Required |
|--------|-----------------------------------|--------------------------------------------------|---------------|
| POST   | `/register`                       | Register new user                                | No            |
| POST   | `/Login`                          | Login (returns JWT + Refresh Token)              | No            |
| POST   | `/confirm-email?token={token}&userId={id}` | Confirm email                       | No            |
| PATCH  | `/change-password`                | Change password (body: Current & New)            | Yes           |
| POST   | `/refresh-token`                  | Refresh JWT using cookie                         | No            |
| POST   | `/forgot-password?email={email}`  | Send password reset email                        | No            |
| POST   | `/reset-password`                 | Reset password (email, token, newPassword)       | No            |
| POST   | `/resend-confirm-email?email={email}` | Resend confirmation email                    | No            |
| GET    | `/get-current-user`               | Get current authenticated user                   | Yes           |
| GET    | `/signin-google`                  | Initiate Google login                            | No            |
| GET    | `/google-response`                | Google callback (internal)                       | No            |
| GET    | `/signin-facebook`                | Initiate Facebook login                          | No            |
| GET    | `/facebook-response`              | Facebook callback (internal)                     | No            |
| POST   | `/logout`                         | Logout (invalidate refresh token)                | Yes           |

### How to Run Locally
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/ventro-api.git
