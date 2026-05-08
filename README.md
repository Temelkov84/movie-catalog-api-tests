# 🎬 Movie Catalog API Test Automation

![CI](https://github.com/Temelkov84/movie-catalog-api-tests/actions/workflows/dotnet.yml/badge.svg)

## 📌 Project Overview

This project demonstrates API test automation skills including REST API validation, independent test design, data management, CI integration, and test reporting.

The system under test is a Movie Catalog REST API that allows users to manage movies (create, edit, retrieve and delete).

---

## 🛠️ Tech Stack

* C#
* NUnit
* RestSharp
* .NET 8
* GitHub Actions (CI/CD)
* Allure Reports

---

## 🚀 Features

* JWT authentication handling
* Full CRUD API test coverage:

  * Create Movie
  * Edit Movie
  * Get All Movies
  * Delete Movie

* Negative test scenarios:

  * Missing required fields
  * Non-existing resources

* Independent tests (no shared state)
* Automatic test data cleanup
* Unique test data using GUID
* Allure test results uploaded as GitHub Actions artifact

---

## 🧪 Test Design

* Each test creates its own data
* Tests do not depend on execution order
* Dynamic test data generation (GUID)
* Cleanup implemented via TearDown
* Assertions validate both status codes and response content

---

## ▶️ How to Run

### Run locally:

```bash
dotnet test