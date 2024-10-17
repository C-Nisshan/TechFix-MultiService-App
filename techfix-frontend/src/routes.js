import React from 'react';
import { Routes, Route } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import Dashboard from './pages/Dashboard';
import Product from './pages/ProductItem';
import RFQ from './pages/RFQItem';
import Supplier from './pages/Supplier';
import User from './pages/User';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import RegistrationPage from './pages/Registration';

const AppRoutes = () => (
  <Routes>
    {/* Public route */}
    <Route path="/" element={<LoginPage />} />
    <Route path= "/registration" element={<RegistrationPage/>}/>
    <Route path="/user-dashboard" element={< Dashboard />} />
    <Route path="/product" element={< Product />} />
    <Route path="/rfq" element={<RFQ />} />
    <Route path="/supplier" element={<Supplier />} />
    <Route path="/user" element={<User/>} />
  </Routes>
);

export default AppRoutes;
