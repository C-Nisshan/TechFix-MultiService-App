// routes.js
import React from 'react';
import { Routes, Route } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import Product from './pages/ProductItem';
import SupplierRFQ from './pages/SupplierRFQ';
import User from './pages/User';
import Client from './pages/Client';
import RegistrationPage from './pages/Registration';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const AppRoutes = () => (
  <Routes>
    <Route path="/" element={<LoginPage />} />
    <Route path="/product" element={<Product />} />
    <Route path="/rfq" element={<SupplierRFQ />} />
    <Route path="/user" element={<User />} />
    <Route path="/client" element={<Client />} />
    <Route path= "/registration" element={<RegistrationPage/>}/>
  </Routes>
);

export default AppRoutes;
