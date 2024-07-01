# BackEnd-C-.NET-WebApi-books-app

This project is a Web API built with C# .NET using a Database-First approach. It provides the backend functionality for a book trading platform where users can register, manage their book listings, and engage in book trading transactions. 
The API handles user authentication, book management, and transaction processes, ensuring a seamless integration with the front-end Angular application.

The Book Trading System is an application designed to facilitate the buying, selling, and trading of books among users. It provides a platform where users can list their books for sale, request to purchase books, and manage their transactions. The system ensures a seamless process for approving sales and confirming the reception of books. 

Book: Supplementary entity of the BookForSale. They connect 1-1. Provides additional information to the BookForSale Entity.

BookForSale:  Represents the entry of a book that a user (seller) wants to sell or donate. Manages the listing and availability of books that users want to sell.

ApprovalSale: Represents a record of a user's request to purchase a book and the subsequent approval process. Tracks and manages the approval process for purchase requests, ensuring that transactions are authorized before proceeding.

BookReadyForTrading: Represents a book that has been approved for sale and is ready for trading between users. Holds the details of books that have passed the approval stage and are now ready for the final trading process.

Sale: Represents the final record of a completed transaction where a book has been sold or traded. Provides a record of completed transactions, ensuring that all sales and exchanges are documented.

Project Workflow
Listing a Book for Sale:

A user (seller) lists a book for sale by creating an entry in the BookForSale table, specifying details such as condition, price, and payment method.
Requesting to Buy a Book:

Another user (buyer) requests to purchase a book by creating an entry in the ApprovalSale table, which starts the approval process.
Approving a Purchase Request:

The seller reviews the purchase request and approves it, moving the book to the BookReadyForTrading table.
Confirming Reception of the Book:

After the buyer receives the book, they confirm the reception, which removes the book from the BookReadyForTrading table and creates an entry in the Sale table, finalizing the transaction.
This structured approach ensures that all steps of the book trading process are tracked and managed effectively, providing a smooth experience for users engaging in buying, selling, and trading books.

There is only one admin, but i have not given functionality yet.

Technologies Used

C# .NET: Main programming language and framework for building the API.
Entity Framework Core: Used for Database-First approach, enabling easy interaction with the database.
ASP.NET Core: Framework for building web APIs.
AutoMapper: For object-to-object mapping, simplifying the transfer of data between layers.
JWT (JSON Web Tokens): For secure user authentication.
SQL Server: Database management system.
This project exemplifies the use of modern .NET technologies to create a robust and scalable backend service for a book trading platform.


For the front-end you can see the  [FrontEnd-angular-books-app](https://github.com/kateKipa/FrontEnd-angular-books-app)

[This is the back-end](https://github.com/kateKipa/BackEnd-C-.NET-WebApi-books-app)
