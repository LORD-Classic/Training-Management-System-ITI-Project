﻿// Training Management System - Enhanced JavaScript

// DOM Content Loaded
document.addEventListener('DOMContentLoaded', function () {
    // Initialize tooltips
    initializeTooltips();
    
    // Initialize animations on scroll
    initializeScrollAnimations();
    
    // Initialize loading states for forms
    initializeFormLoading();
    
    // Initialize table search functionality
    initializeTableSearch();
    
    // Initialize dashboard counters
    initializeDashboardCounters();
    
    // Initialize theme switcher (future feature)
    initializeThemeSwitcher();
});

// Initialize Bootstrap tooltips
function initializeTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Initialize scroll animations
function initializeScrollAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-fade-in-up');
            }
        });
    }, observerOptions);

    // Observe all elements with animation classes
    document.querySelectorAll('.card, .stats-card, .feature-icon').forEach(el => {
        observer.observe(el);
    });
}

// Initialize form loading states
function initializeFormLoading() {
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function(e) {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn && !submitBtn.disabled) {
                submitBtn.innerHTML = '<span class="loading-spinner me-2"></span>Processing...';
                submitBtn.disabled = true;
                
                // Re-enable after 10 seconds as fallback
                setTimeout(() => {
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = submitBtn.getAttribute('data-original-text') || 'Submit';
                }, 10000);
            }
        });
    });
}

// Initialize table search functionality
function initializeTableSearch() {
    const searchInputs = document.querySelectorAll('input[placeholder*="Search"]');
    
    searchInputs.forEach(input => {
        let timeout;
        input.addEventListener('input', function() {
            clearTimeout(timeout);
            timeout = setTimeout(() => {
                // Add search highlight functionality here
                highlightSearchResults(this.value);
            }, 300);
        });
    });
}

// Highlight search results
function highlightSearchResults(searchTerm) {
    if (!searchTerm) return;
    
    const tableRows = document.querySelectorAll('tbody tr');
    tableRows.forEach(row => {
        const cells = row.querySelectorAll('td');
        let hasMatch = false;
        
        cells.forEach(cell => {
            const originalText = cell.textContent;
            if (originalText.toLowerCase().includes(searchTerm.toLowerCase())) {
                hasMatch = true;
                const highlightedText = originalText.replace(
                    new RegExp(searchTerm, 'gi'),
                    '<mark>$&</mark>'
                );
                cell.innerHTML = highlightedText;
            }
        });
        
        row.style.display = hasMatch ? '' : 'none';
    });
}

// Initialize dashboard counters with animation
function initializeDashboardCounters() {
    const counters = document.querySelectorAll('.stats-number');
    
    counters.forEach(counter => {
        const target = parseInt(counter.textContent);
        if (target > 0) {
            animateCounter(counter, target);
        }
    });
}

// Animate counter from 0 to target
function animateCounter(element, target) {
    let current = 0;
    const increment = target / 50; // 50 steps
    const timer = setInterval(() => {
        current += increment;
        if (current >= target) {
            element.textContent = target;
            clearInterval(timer);
        } else {
            element.textContent = Math.ceil(current);
        }
    }, 40); // 40ms per step = 2 second total animation
}

// Initialize theme switcher (placeholder for future feature)
function initializeThemeSwitcher() {
    // This could be used for dark/light theme switching
    const theme = localStorage.getItem('theme') || 'light';
    document.documentElement.setAttribute('data-theme', theme);
}

// Utility functions
function showToast(message, type = 'success') {
    // Create toast notification
    const toastHTML = `
        <div class="toast align-items-center text-white bg-${type} border-0" role="alert">
            <div class="d-flex">
                <div class="toast-body">
                    <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-circle'} me-2"></i>
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;
    
    // Add to toast container or create one
    let toastContainer = document.querySelector('.toast-container');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.className = 'toast-container position-fixed bottom-0 end-0 p-3';
        document.body.appendChild(toastContainer);
    }
    
    toastContainer.insertAdjacentHTML('beforeend', toastHTML);
    const toastElement = toastContainer.lastElementChild;
    const toast = new bootstrap.Toast(toastElement);
    toast.show();
    
    // Remove from DOM after hiding
    toastElement.addEventListener('hidden.bs.toast', () => {
        toastElement.remove();
    });
}

// Confirm delete actions
function confirmDelete(event, itemName = 'item') {
    event.preventDefault();
    
    if (confirm(`Are you sure you want to delete this ${itemName}? This action cannot be undone.`)) {
        window.location.href = event.target.href;
    }
}

// Auto-dismiss alerts after 5 seconds
document.querySelectorAll('.alert:not(.alert-permanent)').forEach(alert => {
    setTimeout(() => {
        const bsAlert = new bootstrap.Alert(alert);
        bsAlert.close();
    }, 5000);
});

// Smooth scrolling for anchor links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Form validation enhancement
function enhanceFormValidation() {
    document.querySelectorAll('.form-control').forEach(input => {
        input.addEventListener('blur', function() {
            validateField(this);
        });
        
        input.addEventListener('input', function() {
            if (this.classList.contains('is-invalid')) {
                validateField(this);
            }
        });
    });
}

function validateField(field) {
    if (field.checkValidity()) {
        field.classList.remove('is-invalid');
        field.classList.add('is-valid');
    } else {
        field.classList.remove('is-valid');
        field.classList.add('is-invalid');
    }
}

// Initialize enhanced form validation
enhanceFormValidation();

// Custom client-side validation for date comparisons
jQuery.validator.addMethod("futuredate", function (value, element) {
    if (!value) return true;
    var today = new Date();
    var inputDate = new Date(value);
    return inputDate >= today.setHours(0, 0, 0, 0);
}, "Date cannot be in the past");

jQuery.validator.addMethod("dategreaterthan", function (value, element, params) {
    if (!value) return true;
    var startDateValue = $(params).val();
    if (!startDateValue) return true;
    
    var startDate = new Date(startDateValue);
    var endDate = new Date(value);
    return endDate > startDate;
}, "End date must be after start date");

// Add unobtrusive validation adapters
jQuery.validator.unobtrusive.adapters.add("futuredate", function (options) {
    options.rules["futuredate"] = true;
    options.messages["futuredate"] = options.message;
});

jQuery.validator.unobtrusive.adapters.add("dategreaterthan", ["other"], function (options) {
    options.rules["dategreaterthan"] = "#" + options.params.other;
    options.messages["dategreaterthan"] = options.message;
});

// Auto-submit forms on filter changes
$(document).ready(function () {
    // Auto-submit filter forms
    $('select[name="filterByRole"], select[name="filterByTraineeId"], select[name="filterBySessionId"]').on('change', function () {
        $(this).closest('form').submit();
    });
    
    // Initialize datetime-local inputs with proper format
    $('input[type="datetime-local"]').each(function() {
        if (!$(this).val()) {
            var now = new Date();
            now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
            $(this).val(now.toISOString().slice(0, 16));
        }
    });
});
