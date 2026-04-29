# 🎬 Movie Catalog API Test Automation

## 📌 Project Overview

This project contains automated API tests for a Movie Catalog REST API.
The goal is to demonstrate practical skills in back-end testing, test automation, and CI integration.

---

## 🛠️ Tech Stack

* C#
* NUnit
* RestSharp
* .NET 8
* GitHub Actions (CI)

---

## 🚀 Features

* Authentication with JWT token
* CRUD operations testing:

  * Create Movie
  * Edit Movie
  * Get All Movies
  * Delete Movie
* Negative test scenarios:

  * Invalid data
  * Non-existing resources
* Independent tests (no shared state)
* Automatic test data cleanup

---

## 🧪 Test Design

* Each test creates its own test data
* No dependency between tests
* Unique test data using GUID
* Cleanup implemented using TearDown

---

## ▶️ How to Run

### Run locally:

```bash
dotnet test
```

### Run via GitHub Actions:

Tests run automatically on push.

---

## 📊 CI Integration

* GitHub Actions workflow
* Automatic build and test execution

---

## 📂 Project Structure

```
Exam_Movie/
 ├── Clients/
 ├── Configuration/
 ├── DTOs/
 ├── Helpers/
 └── Tests/
```

---

## 🎯 Purpose

This project is part of a QA Automation portfolio, focusing on API testing and clean test design.

---
