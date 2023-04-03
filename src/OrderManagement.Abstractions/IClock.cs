namespace OrderManagement.Abstractions;

public interface IClock
{
    DateTime CurrentDate();
    DateTime CurrentServerDate();
}