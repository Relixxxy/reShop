# reShop
This repository contains the implementation of a microservice architecture project in .NET. The architecture of this project is composed of several microservices and other components, such as an MVC web application, an Identity Provider, a Proxy/Cdn, a Provider Cache, and a Database Server.

## Architecture Overview
### The project consists of the following microservices:

Catalog: This microservice is responsible for managing product catalogs.\
Basket: This microservice is responsible for managing user shopping baskets.\
Order: This microservice is responsible for managing orders placed by users.
### Other components of the project include:

MVC: This is a web application that serves as the user interface of the project.\
Identity Provider (Identity Server 4): This component is responsible for providing authentication and authorization services.\
Proxy/Cdn (nginx): This component acts as a reverse proxy and a content delivery network to enhance the performance and scalability of the project.\
Provider Cache (redis): This component is responsible for caching data and improving the performance of the project.\
DataBase Server (postgreSQL): This is the database server used by the project to store and retrieve data.
