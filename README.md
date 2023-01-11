# api-authentication-middleware
A simple demonstration how to use middleware to secure an API in .Net 6 Core.

A username and password has to be supplied in the request header. This is checked against an AWS DynamoDb before any action can be performed, otherwise you get a 401 Unauthorized Access error.
