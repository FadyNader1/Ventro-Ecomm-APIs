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

### Key Features

#### Product Management
- Get all products with **pagination, filtering, sorting**
- Add / Update / Delete products with multiple image uploads
- Get product by ID
- Home page data: Latest, Featured, and Offer products

#### Wishlist
- Add / Remove product from wishlist `[Authorize]`
- Get current user's wishlist
- Check if a product is in the wishlist

#### Shopping Basket (Redis-based)
- Create new basket ID
- Add / Remove / Clear items
- Retrieve basket by ID

#### Orders
- Create order from basket `[Authorize]`
- Get order by ID (user-specific)
- Get all orders for current user
- Download order invoice as **PDF**

#### Delivery Methods
- Full CRUD operations (Admin-facing)

#### Categories
- Full CRUD operations

#### Payment
- Create Stripe Payment Intent using basket ID

#### Authentication & Authorization
- Register / Login with JWT + Refresh Token (stored in HttpOnly cookie)
- Email confirmation
- Change password / Forgot password / Reset password
- Refresh token endpoint
- Get current user info
- External login with **Google** and **Facebook**
- Logout

### Main Controllers
- `ProductController`
- `WishListController`
- `BasketController`
- `OrderController`
- `PaymentController`
- `DeliveryMethodController`
- `CategoryController`
- `AuthController`

### API Response Format
All endpoints return consistent responses using a custom `ApiResponse<T>` wrapper:
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { ... },
  "meta": { ... } // for paginated responses
}
