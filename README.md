Bankaları bir entity olarak değil, constant yapılar şeklinde tanımladım. Bunun nedeni, bankaların (örneğin Akbank, Garanti ve Yapı Kredi) doğrudan sistemin domain’ine ait olmaktan ziyade, external servislerle  entegre çalışan yapılardır. 
projede onları birer extension (gateway) mantığında ele almak daha doğru bir yaklaşım olacaktır.
