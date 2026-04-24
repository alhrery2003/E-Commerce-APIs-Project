# E-Commerce APIs

## Overview
The E-Commerce API is a robust backend system designed to manage core e-commerce functionalities. It enables users to browse products, manage their shopping carts, and place orders seamlessly. 

The system supports:
- **User Authentication and Authorization**
- **Product Browsing** with advanced filtering and pagination
- **Cart Management**
- **Order Processing**

## Architecture & Technical Requirements
This project follows clean architecture principles and is built using modern .NET technologies. The architecture ensures scalability, maintainability, and testability.

### Key Patterns & Practices
- **N-Tier Architecture**: Divided into 3 Core Layers + Common.
- **Repository Pattern**: Extensively using both Generic and Non-Generic repositories.
- **Unit of Work**: Ensures transactional consistency across database operations.
- **Data Transfer Objects (DTOs)**: Used for data encapsulation and security.
- **Fluent Validation**: For comprehensive and clean request validation.
- **Asynchronous Programming**: End-to-end async/await for optimal performance.
- **Result Pattern**: Implements a general response wrapper for consistent API responses.
- **Entity Framework Core (EF Core)**: Used as the primary ORM.

### Security & Access Control
- **JWT Authentication**: Secure token-based authentication.
- **Policy-Based Authorization**: Fine-grained access control.
- **Microsoft Identity**: For robust identity management.
- **CORS Enabled**: Configured to safely accept cross-origin requests.

> **Note**: User context is securely handled. `UserId` is always extracted directly from JWT Claims and is **never** passed in client requests.

---

## Main Components
1. **Users**
2. **Products**
3. **Categories**
4. **Cart**
5. **Orders**

---

## API Endpoints

### User Management
- `POST /api/auth/register` - Create a new user account.
- `POST /api/auth/login` - Authenticate user and provide a JWT token.

### Category Management
- `GET /api/categories` - Get all categories.
- `GET /api/categories/{id}` - Get category details.
- `POST /api/categories` - Create a new category.
- `PUT /api/categories/{id}` - Update a category.
- `DELETE /api/categories/{id}` - Delete a category.

### Product Management
- `GET /api/products?categoryId=&name=&pageNumber=&pageSize=` - Get products with filtering, search, and pagination.
- `GET /api/products/{id}` - Get product details.
- `POST /api/products` - Create a new product.
- `PUT /api/products/{id}` - Update a product.
- `DELETE /api/products/{id}` - Delete a product.

### Cart Management
- `GET /api/cart` - Get user cart.
- `POST /api/cart` - Add an item to the cart.
- `PUT /api/cart` - Update cart item quantity.
- `DELETE /api/cart/{productId}` - Remove an item from the cart.

### Order Management
- `GET /api/orders` - View order history.
- `GET /api/orders/{id}` - Get order details.
- `POST /api/orders` - Place a new order.

### File Management
- `POST /api/image/upload` - General image upload.
- `POST /api/products/{id}/image` - Upload a product image.
- `POST /api/categories/{id}/image` - Upload a category image.

---

## Important Notes for Developers
- **Backend Only**: This repository contains the backend API services; no UI is included.
- **Clean Architecture**: Strictly adhere to the separation of concerns and clean architecture principles.
- **Database**: Ensure EF Core migrations are updated when modifying domain entities.
- **Security**: Never pass `UserId` in the request body or query string. It must be resolved securely via JWT claims in the application pipeline.
