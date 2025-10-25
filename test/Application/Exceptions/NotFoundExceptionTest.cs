using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Exceptions;

namespace uServiceDemo.Application.Test.Exceptions;

[TestClass]
public class NotFoundExceptionTest
{
    [TestMethod]
    public void Should_Create_Exception_With_Default_Constructor()
    {
        var exception = new NotFoundException();
        
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(Exception));
    }

    [TestMethod]
    public void Should_Create_Exception_With_Message()
    {
        var message = "Resource not found";
        var exception = new NotFoundException(message);
        
        Assert.AreEqual(message, exception.Message);
    }

    [TestMethod]
    public void Should_Create_Exception_With_Message_And_InnerException()
    {
        var message = "Resource not found";
        var innerException = new InvalidOperationException("Inner error");
        var exception = new NotFoundException(message, innerException);
        
        Assert.AreEqual(message, exception.Message);
        Assert.AreEqual(innerException, exception.InnerException);
    }

    [TestMethod]
    public void Should_Be_Throwable()
    {
        try
        {
            throw new NotFoundException("Test exception");
        }
        catch (NotFoundException ex)
        {
            Assert.IsNotNull(ex);
            Assert.AreEqual("Test exception", ex.Message);
        }
    }

    [TestMethod]
    public void Should_Be_Catchable_As_Base_Exception()
    {
        try
        {
            throw new NotFoundException("Test");
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex, typeof(NotFoundException));
        }
    }
}
